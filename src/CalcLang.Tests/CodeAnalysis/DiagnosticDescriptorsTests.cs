using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CalcLang.CodeAnalysis {
    public sealed class DiagnosticDescriptorsTests {

        [Fact]
        public void Descriptors__Unique() {
            var descriptors = typeof( DiagnosticDescriptors ).GetProperties( BindingFlags.Public | BindingFlags.Static )
                .Where( p => p.PropertyType == typeof( DiagnosticDescriptor ) )
                .Select( p => p.GetValue( null ) )
                .Cast<DiagnosticDescriptor>();
            Assert.NotNull( descriptors );
            Assert.NotEmpty( descriptors );

            var set = new HashSet<string>();
            foreach( var dd in descriptors) {
                Assert.DoesNotContain( dd.Id, set );
                set.Add( dd.Id );
            }
        }

    }
}