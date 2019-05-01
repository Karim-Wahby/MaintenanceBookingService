var DeliveredRequestsViewManager = function (
    tableContainerIdDetails,
    tableMarkDeliveredButtonTemplateId)
{
    var apiManager = new APIManager();

    var ServiceRequestUtilites = new ServiceRequestHelper();

    var DeliveredRequestsTable = new TableManager(
        tableContainerIdDetails,
        {
            order: [[4, "desc"], [0, "desc"]],
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
                    data: 'DescriptionOfRequiredService'
                },
                {
                    data: 'TimeOfServiceDelivery',
                    render: function (serviceDateTime, unUsed, requestObject, tableObject) {
                        return ServiceRequestUtilites.GetDateFromServiceDateTime(serviceDateTime);
                    }
                },
                {
                    data: 'Rating',
                    render: function (serviceRating, unUsed, requestObject, tableObject) {
                        return ServiceRequestUtilites.RatingToHtml(serviceRating);
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
                    width: '25%'
                },
                {
                    targets: 2,
                    width: '40%'
                },
                {
                    targets: 3,
                    width: '15%'
                },
                {
                    targets: 4,
                    width: '10%'
                }
            ]
        });
    
    var GetDeliveredRequests = function () {
        DeliveredRequestsTable.Hide();
        DeliveredRequestsTable.StartLoadingWheel();
        apiManager.get(
            "MaintenanceServicesRequests",
            "DeliveredRequests",
            {},
            function (allDeliveredRequests) {
                DeliveredRequestsTable.Rerender(allDeliveredRequests);
                DeliveredRequestsTable.StopLoadingWheel();
                DeliveredRequestsTable.Show();
            });
    };

    return {
        DisplayAllDeliveredRequests: GetDeliveredRequests
    };
};
