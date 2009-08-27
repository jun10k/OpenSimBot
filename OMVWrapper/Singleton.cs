
namespace OpenSimBot.OMVWrapper.Utility
{

    public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get { return SingletonCreator.instance; }
        }

        private class SingletonCreator
        {
            static SingletonCreator() { }
            internal static readonly T instance = new T();
        }
    }
}