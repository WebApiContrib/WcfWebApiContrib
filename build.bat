cls
msbuild .\wcf\Http\Http.sln
msbuild .\wcf\WCFJQuery\WcfJQuery.sln
msbuild .\wcf\Http\Http.sln /p:Configuration=Release
msbuild .\wcf\WCFJQuery\WcfJQuery.sln /p:Configuration=Release
msbuild .\Samples\Samples.sln
