public interface ITurnable
{
    void Turn();
    // High Priority is done first, low priority done last.
    int Prio();
}