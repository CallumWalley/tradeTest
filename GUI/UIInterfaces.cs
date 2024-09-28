using System.Numerics;

public partial class UIInterfaces
{
    // Indicates can receive signal to update game elements.
    public interface IEFrameUpdatable
    {
        void OnEFrameUpdate();
    }

}
