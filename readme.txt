Welcome to WCF Web API Contrib


Contents as of 2011-05-07

Reorganization to allow a WebAPIContrib Nuget to be built so people can actually use the bits as well as just look at samples.  Just as a warning, these bits are highly likely to change, so use them for experimental purposes only :-).

build.bat            
- Rebuilds all projects including the source for WCF Web API

wcf                  
- Mirror subrepository of the main WCF Web API Codeplex project

Samples              - Various projects that demonstrate capabilities of WCF Web API and the extensions included in this project
 ContactManager      - Copy of ContactManager sample from Preview 3 that was adapted to use different templating engines
 SelfHostedServer    - Self hosted service that demonstrates using a DI container and Logging Operation Handler (now using Preview 4)
 FSharpServer        - FSharp host using Preview 3
 Silverlight         - Silverlight example client and server  (Using Preview 3)

Source               - Code that will go into the NuGet
 Formatters          - Reusable classes that serialize and deserialize types into wire representations.
 MessageHandlers     - Reusable message handlers
 OperationHandlers   - Reusable operation handlers

Security             - Pablo's Security project.  If he is ok with it, this will be replaced by the Security folder in the MessageHandler project

libs                 - Home of third party libraries used by this probject.  Probably will be replaced by Nuget packages at some point.

tools         


