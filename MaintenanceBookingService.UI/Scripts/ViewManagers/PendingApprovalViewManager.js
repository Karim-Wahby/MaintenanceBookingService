﻿var PendingApprovalViewManager = function (
    tableContainerIdDetails,
    tableApproveButtonTemplateId,
    serviceDetailsViewManager)
{
    var apiManager = new APIManager();

    var ServiceRequestUtilites = new ServiceRequestHelper();

    var pendingApprovalRequestsTable = new TableManager(
        tableContainerIdDetails,
        {
            order: [[2, "desc"], [3, "desc"]],
            createdRow: function (row, data, index) {
                row.id = GetRequestRowId(data.Id);
            },
            columns: [
                {
                    data: 'Id'
                },
                {
                    data: 'MaintenanceServiceNeeded',
                    render: function (neededService, unUsed, requestObject, tableObject) {
                        return ServiceRequestUtilites.MaintenanceServiceToHtml(neededService);
                    }
                },
                {
                    data: 'TimeOfServiceDelivery',
                    render: function (serviceDateTime, unUsed, requestObject, tableObject) {
                        return ServiceRequestUtilites.GetDateFromServiceDateTime(serviceDateTime);
                    }
                },
                {
                    data: 'TimeOfServiceDelivery',
                    render: function (serviceDateTime, unUsed, requestObject, tableObject) {
                        return ServiceRequestUtilites.GetTimeFromServiceDateTime(serviceDateTime);
                    }
                },
                {
                    data: 'Id',
                    render: function (requestId, unUsed, requestObject, tableObject) {
                        return $("#" + tableApproveButtonTemplateId)
                            .html()
                            .replace("__OnClickEventHandlerData__", "'" + requestId + "'")
                            .replace("__id__", GetRequestApproveButtonId(requestId));
                    }
                }
            ],
            columnDefs: [
                {
                    targets: 0,
                    width: '10%'
                },
                {
                    targets: 1,
                    width: '30%'
                },
                {
                    targets: 2,
                    width: '30%'
                },
                {
                    targets: 3,
                    width: '20%'
                },
                {
                    targets: 4,
                    width: '10%'
                }
            ]
        });

    var GetRequestApproveButtonId = function (requestId) {
        return 'Request_' + requestId + '_ApproveButton';
    };

    var GetRequestRowId = function (requestId) {
        return 'PendingApprovalRequest_' + requestId + '_Row';
    };

    var ApprovePendingRequest = function (requestId) {
        var param = {};
        param['id'] = requestId;
        apiManager.get(
            "MaintenanceServicesRequests",
            "ApproveRequest",
            param,
            function (success) {
                if (success) {
                    pendingApprovalRequestsTable.RemoveRow(GetRequestRowId(requestId));
                    alert("user request approved successfully!!");
                }
                else {
                    alert("failed to approve user request");
                }
            });
    };

    var GetPendingRequests = function () {
        pendingApprovalRequestsTable.Hide();
        pendingApprovalRequestsTable.StartLoadingWheel();
        apiManager.get(
            "MaintenanceServicesRequests",
            "PendingApproval",
            {},
            function (allPendingApprovalRequests) {
                pendingApprovalRequestsTable.Rerender(
                    allPendingApprovalRequests,
                    serviceDetailsViewManager.GetAndDisplayServiceDetails,
                    function (serviceDetails) { return serviceDetails.Id; });
                pendingApprovalRequestsTable.StopLoadingWheel();
                pendingApprovalRequestsTable.Show();
            });
    };

    return {
        ApproveRequest: ApprovePendingRequest,
        DisplayAllPendingRequests: GetPendingRequests
    };
};
