If you want to publish or use the project, you need to be aware of the following things:
1. The algrithm is built on X64, so you should rebuild it on X64 platform and use the 64bit version of IIS Express.
2. Service1.svc is used for our phone project. Service2.svc is WCF service with ajax-enabled, so you can't use it for our project.
3. In Web.config, The values of "ImgDirectory" and "ReturnURL" need to be modified based on your environment settings.
4. You can modify the algrithm on the folder Source.

It works now. But I have no experiences on building WCF or WEBAPI before, So If you have any suggestions or questions, please contact v-zhwe@microsoft.com.