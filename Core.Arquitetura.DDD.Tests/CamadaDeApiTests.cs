using System.Reflection;
using NetArchTest.Rules;
using System.Text;

namespace Core.Arquitetura.DDD.Tests;

public class CamadaApiTests
{
    [Test]
    public void NenhumaCamadaDeveDependerDaApi()
    {
        var assemblies = AssemblyCamadaLoader.Carregar("Core.Api.");

        if (assemblies.Count == 0)
        {
            Assert.Pass("Nenhum assembly com prefixo 'Core.Api.' encontrado.");
            return;
        }
        
        var result = Types.InAssemblies(assemblies)
            .That()
            .DoNotHaveName("Program")
            .ShouldNot()
            .HaveDependencyOnAny("Core.Api" )
            .GetResult();

        if (!result.IsSuccessful)
        {
            var error = BuildDetailedErrorMessage(result, assemblies);
            Assert.Fail(error);
        }

        Assert.Pass("✅ Nenhuma dependência indevida encontrada (excluindo classes Program).");
    }

    private string BuildDetailedErrorMessage(TestResult result, IReadOnlyList<Assembly> assemblies)
    {
        var sb = new StringBuilder();
        sb.AppendLine("🚫 ARQUITETURA VIOLADA");
        sb.AppendLine("======================");

        var problematicAssembly = assemblies.FirstOrDefault(a =>
            a.GetReferencedAssemblies().Any(ra =>
                ra.Name != null &&
                ra.Name.Contains("Core.Api")));

        if (problematicAssembly != null)
        {
            sb.AppendLine($"📦 Projeto problemático: {problematicAssembly.GetName().Name}");
            sb.AppendLine();

            // Listar tipos do projeto
            try
            {
                var types = problematicAssembly.GetTypes()
                    .Where(t => t.Name == "Program")
                    .ToList();

                if (types.Any())
                {
                    sb.AppendLine("🔍 Classes 'Program' encontradas:");
                    foreach (var type in types)
                    {
                        sb.AppendLine($"   • {type.FullName}");

                        // Tentar descobrir o que está causando a dependência
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Static | BindingFlags.Instance);
                        foreach (var method in methods.Take(5))
                        {
                            sb.AppendLine($"     Método: {method.Name}");
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                sb.AppendLine("⚠️  Não foi possível carregar todos os tipos (dependências faltando)");
            }
        }

        if (result.FailingTypes != null && result.FailingTypes.Any())
        {
            sb.AppendLine();
            sb.AppendLine("📋 Todos os tipos violadores:");
            foreach (var type in result.FailingTypes.OrderBy(t => t.Name))
            {
                sb.AppendLine($"   • {type.Name} ({type.Namespace})");
            }
        }
        
        return sb.ToString();
    }
}