require.config({
    baseUrl: '',
    urlArgs: 'v=1.2.81.1'
});

require(
    [
        'Scripts/angular-local-storage.min',
         'app',
         'Scripts/routeResolver',
         'Client/JSControllers/Index/IndexController',
         'Client/OtherServices/httpInterceptor', 
         'Client/OtherServices/AuthService',
         'Client/OtherServices/MessageService',
         'Client/JsonServices/Dashboard/DashboardService',
         'Client/JsonServices/Companies/CompaniesService',
         'Client/JsonServices/Roles/RoleService',
         'Client/JsonServices/Vendor/VendorService',
         'Client/JsonServices/Job/JobService',
         'Client/JsonServices/Users/UserService',
         'Client/JsonServices/LedgerAccounts/LedgerAccountsService',
         'Client/JsonServices/Account/AccountService',
         'Client/JsonServices/AchSetupService/AchSetupService',
         'Client/JsonServices/RemoteApplicationHost/RemoteApplicationHostService',
         'Client/JsonServices/PurchaseOrder/PurchaseOrderService',
         'Client/JsonServices/RemoteSync/RemoteSyncService',
         'Client/JsonServices/SyncBatche/SyncBatcheService',
         'Client/JsonServices/ApprovalCriteria/ApprovalCriteriaService',
         'Client/JsonServices/Attachment/AttachmentService',
         'Client/JsonServices/Ticket/TicketService',
         'Client/JsonServices/Invoice/InvoiceService',
         'Client/JsonServices/SettingsService/IniSettingService',
         'Client/JsonServices/EmailTemplate/EmailTemplateService',
         'Client/JsonServices/IoneAppEventService/IoneAppEventService',
         'Client/JsonServices/ResetPwd/ResetPwdService',
         'Client/JsonServices/Master/MasterService',
         ''
         
    ],
    function ()
    {
        angular.bootstrap(document, ['IOneApp']);
    });
 