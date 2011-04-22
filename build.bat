cls
msbuild .\"wcf\WCFWebAPI\WCF Web API - All.sln"
msbuild .\wcf\WCFJQuery\WcfJQuery.sln
msbuild .\"wcf\WCFWebAPI\WCF Web API - All.sln" /p:Configuration=Release
msbuild .\wcf\WCFJQuery\WcfJQuery.sln /p:Configuration=Release
msbuild .\Samples\Samples.sln
