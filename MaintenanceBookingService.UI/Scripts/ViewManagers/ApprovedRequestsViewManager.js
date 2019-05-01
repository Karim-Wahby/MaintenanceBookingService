var ApprovedRequestsViewManager = function (
    tableContainerIdDetails,
    tableMarkDeliveredButtonTemplateId,
    serviceDetailsViewManager)
{
    var apiManager = new APIManager();

    var ServiceRequestUtilites = new ServiceRequestHelper();

    var ApprovedRequestsTable = new TableManager(
        tableContainerIdDetails,
        {
            order: [[2, "desc"], [3, "desc"]],
            createdRow: function (row, data, index) {
                row.id = GetServiceIdRowId(data.Id);
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
                        return $("#" + tableMarkDeliveredButtonTemplateId)
                            .html()
                            .replace("__OnClickEventHandlerData__", "'" + requestId + "'")
                            .replace("__id__", GetMarkDeliveredButtonId(requestId));
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

    var GetMarkDeliveredButtonId = function (requestId) {
        return 'Request_' + requestId + '_MarkDeliveredButton';
    };

    var GetServiceIdRowId = function (requestId) {
        return 'ApprovedRequest_' + requestId + '_Row';
    };

    var MaskRequestAsDelivered = function (serviceId) {
        var param = {};
        param['id'] = serviceId;
        apiManager.get(
            "MaintenanceServicesRequests",
            "FinalizeRequest",
            param,
            function (success) {
                if (success) {
                    ApprovedRequestsTable.RemoveRow(GetServiceIdRowId(serviceId));
                    alert("user Service Marked as Delivered!!");
                }
                else {
                    alert("failed to Marked  user Service as Delivered");
                }
            });
    };

    var GetApprovedRequests = function () {
        ApprovedRequestsTable.Hide();
        ApprovedRequestsTable.StartLoadingWheel();
        apiManager.get(
            "MaintenanceServicesRequests",
            "ApprovedRequests",
            {},
            function (allApprovedRequests) {
                ApprovedRequestsTable.Rerender(
                    allApprovedRequests,
                    serviceDetailsViewManager.GetAndDisplayServiceDetails,
                    function (serviceDetails) { return serviceDetails.Id; });
                ApprovedRequestsTable.StopLoadingWheel();
                ApprovedRequestsTable.Show();
            });
    };

    return {
        MaskRequestAsDelivered: MaskRequestAsDelivered,
        DisplayAllApprovedRequests: GetApprovedRequests
    };
};
