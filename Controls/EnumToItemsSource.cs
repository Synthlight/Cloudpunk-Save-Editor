using System;
using System.Linq;
using System.Windows.Markup;

namespace Save_Editor.Controls {
    public class EnumToItemsSource : MarkupExtension {
        private readonly Type type;

        public EnumToItemsSource(Type type) {
            this.type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return Enum.GetValues(type)
                       .Cast<object>()
                       .Select(e => new {Value = (int) e, DisplayName = e.ToString()});
        }
    }
}