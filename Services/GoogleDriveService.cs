using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskFlow.Wasm.Models;
namespace TaskFlow.Wasm.Services;
public class GoogleDriveService {
 const string FileName="personal-task-tracker.json";
 readonly HttpClient _http; readonly GoogleAuthService _auth;
 public GoogleDriveService(HttpClient http,GoogleAuthService auth){_http=http;_auth=auth;}
 async Task<string> Token(){var t=await _auth.GetTokenAsync(); if(string.IsNullOrWhiteSpace(t)) throw new InvalidOperationException("Please sign in."); return t;}
 async Task<HttpRequestMessage> Request(HttpMethod method,string url){
  var r=new HttpRequestMessage(method,url); r.Headers.Authorization=new AuthenticationHeaderValue("Bearer",await Token()); return r;
 }
 async Task<string?> FileId(){
  var q=Uri.EscapeDataString($"name='{FileName}' and trashed=false");
  var r=await Request(HttpMethod.Get,$"https://www.googleapis.com/drive/v3/files?spaces=appDataFolder&q={q}&fields=files(id,name)");
  var x=await _http.SendAsync(r); x.EnsureSuccessStatusCode();
  using var doc=JsonDocument.Parse(await x.Content.ReadAsStringAsync());
  return doc.RootElement.GetProperty("files").EnumerateArray().FirstOrDefault().TryGetProperty("id",out var id)?id.GetString():null;
 }
 public async Task<List<TaskItem>> GetTasksAsync(){
  var id=await FileId(); if(id==null)return new();
  var r=await Request(HttpMethod.Get,$"https://www.googleapis.com/drive/v3/files/{id}?alt=media");
  var x=await _http.SendAsync(r); x.EnsureSuccessStatusCode();
  return JsonSerializer.Deserialize<List<TaskItem>>(await x.Content.ReadAsStringAsync(),new JsonSerializerOptions{PropertyNameCaseInsensitive=true})??new();
 }
 public async Task SaveTasksAsync(List<TaskItem> tasks){
  var id=await FileId(); var json=JsonSerializer.Serialize(tasks,new JsonSerializerOptions{WriteIndented=true});
  if(id==null){
   var boundary="taskflow_"+Guid.NewGuid().ToString("N");
   var body=$"--{boundary}\r\nContent-Type: application/json; charset=UTF-8\r\n\r\n{{\"name\":\"{FileName}\",\"parents\":[\"appDataFolder\"]}}\r\n--{boundary}\r\nContent-Type: application/json\r\n\r\n{json}\r\n--{boundary}--";
   var r=await Request(HttpMethod.Post,"https://www.googleapis.com/upload/drive/v3/files?uploadType=multipart");
   r.Content=new StringContent(body,Encoding.UTF8); r.Content.Headers.ContentType=MediaTypeHeaderValue.Parse($"multipart/related; boundary={boundary}");
   (await _http.SendAsync(r)).EnsureSuccessStatusCode();
  } else {
   var r=await Request(HttpMethod.Patch,$"https://www.googleapis.com/upload/drive/v3/files/{id}?uploadType=media");
   r.Content=new StringContent(json,Encoding.UTF8,"application/json"); (await _http.SendAsync(r)).EnsureSuccessStatusCode();
  }
 }
}