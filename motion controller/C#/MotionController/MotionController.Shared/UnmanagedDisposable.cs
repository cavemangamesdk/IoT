namespace MotionController;

public abstract class UnmanagedDisposable : Disposable
{
    ~UnmanagedDisposable()
    {
        Dispose(false);
    }
}