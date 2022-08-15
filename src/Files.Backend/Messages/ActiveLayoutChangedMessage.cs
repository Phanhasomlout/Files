using CommunityToolkit.Mvvm.Messaging.Messages;
using Files.Sdk.ViewModels.Layouts;

namespace Files.Sdk.Messages
{
    public sealed class ActiveLayoutChangedMessage : ValueChangedMessage<BaseLayoutViewModel>
    {
        public ActiveLayoutChangedMessage(BaseLayoutViewModel value)
            : base(value)
        {
        }
    }
}
