namespace GameCtor.UITestKit
{
    public partial class UITest
    {
        protected abstract class Condition
        {
            public abstract bool IsFulfilled();

            public abstract string GetResult();
        }
    }
}
