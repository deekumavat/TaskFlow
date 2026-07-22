namespace TaskFlow.Wasm.Models;
public enum TaskState { Open, InProgress, Complete }
public enum PaymentState { NotReceived, Received }
public class TaskItem {
 public Guid Id {get;set;}=Guid.NewGuid();
 public string Name {get;set;}="";
 public string Details {get;set;}="";
 public DateTime StartDate {get;set;}=DateTime.Today;
 public DateTime? EndDate {get;set;}
 public TaskState Status {get;set;}=TaskState.Open;
 public decimal CommercialAmount {get;set;}
 public PaymentState PaymentStatus {get;set;}=PaymentState.NotReceived;
 public DateTime? ReceivedDate {get;set;}
}