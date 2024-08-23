public interface IGameManager {
 ManagerStatus status {get;}
 void Startup(NetworkService service);
}


public enum ManagerStatus {
 Shutdown,
 Initializing,
 Started
}