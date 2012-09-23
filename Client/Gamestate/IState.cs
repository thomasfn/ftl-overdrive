
namespace FTLOverdrive.Client.Gamestate
{
    public interface IState
    {
        void OnActivate();
        void OnDeactivate();

        void Think(float delta);
    }
}
