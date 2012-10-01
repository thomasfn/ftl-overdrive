
namespace FTLOverdrive.Client.Gamestate
{
    public enum RenderStage { PREGUI, POSTGUI }

    public interface IRenderable
    {
        void Render(RenderStage stage);
    }
}
