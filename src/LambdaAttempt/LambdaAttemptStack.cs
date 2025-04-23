using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;
using AssetOptions = Amazon.CDK.AWS.S3.Assets.AssetOptions;
using Runtime = Amazon.CDK.AWS.Lambda.Runtime;

namespace LambdaAttempt
{
    public class LambdaAttemptStack : Stack
    {
        internal LambdaAttemptStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var buildOption = new BundlingOptions()
            {
                Image = Runtime.DOTNET_8.BundlingImage,
                User = "root",
                OutputType = BundlingOutput.ARCHIVED,
                Command = new string[]
                {
                    "/bin/sh",
                    "-c",
                    "dotnet tool install -g Amazon.Lambda.Tools" +
                    " && dotnet build" +
                    " && dotnet lambda package --output-package /asset-output/function.zip"
                }
            };

            var helloWorldLambdaFunction = new Function(this, "LambdaTestFunction", new FunctionProps
            {
                Runtime = Runtime.DOTNET_8,
                MemorySize = 1024,
                LogRetention = RetentionDays.ONE_DAY,
                Handler = "LambdaTestFunction::LambdaTestFunction.Function::FunctionHandler",
                Code = Code.FromAsset("./src/LambdaTestFunction/src/LambdaTestFunction", new AssetOptions()
                {
                    Bundling = buildOption
                }),
            });
        }
    }
}
