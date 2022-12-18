public class RoomDTOResponse
{
  public bool succeeded { get; set; }
  public string message { get; set; }
  public object[] errors { get; set; }
  public RoomDTO data { get; set; }
}