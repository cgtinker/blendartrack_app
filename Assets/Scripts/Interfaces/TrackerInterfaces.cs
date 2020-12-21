namespace ArRetarget
{
    //init method for handlers
    public interface IInit<T>
    {
        void Init(T path);
    }

    //used if data can be received
    public interface IGet<T, C>
    {
        void GetFrameData(T frame, C lastFrame);
    }

    //interface for json string
    public interface IJson
    {
        string j_String();
    }

    //interface for json prefix
    public interface IPrefix
    {
        string j_Prefix();
    }

    //used if data gets pushed
    public interface IStop
    {
        void StopTracking();
    }
}
