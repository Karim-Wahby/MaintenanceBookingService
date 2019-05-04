var TableContainerIdDetails = function (
    loadingWheelId,
    tableContainerId,
    tableId)
{
    return {
        LoadingWheelId: loadingWheelId,
        TableContainerId: tableContainerId,
        TableId: tableId
    };
};

var TableManager = function (tableContainerIdDetails, tableSettings) {
    var loadingwheelmanager = LoadingWheelManager(tableContainerIdDetails.LoadingWheelId);
    var containerSelector = '#' + tableContainerIdDetails.TableContainerId;

    tableSettings.drawCallback = function (settings) {
        $(".dataTables_scrollHeadInner").css({ "width": "100%" });
        $(".table ").css({ "width": "100%" });
    };
    tableSettings.pageLength = 8;
    tableSettings.autoWidth = true;
    tableSettings.pagingType = "simple";

    var table = $('#' + tableContainerIdDetails.TableId).DataTable(tableSettings);

    var hideTable = function () {
        $(containerSelector).addClass("hidden");
    };

    var showTable = function () {
        $(containerSelector).removeClass("hidden");
    };

    var startLoadingWheel = function () {
        loadingwheelmanager.start();

    };

    var stopLoadingWheel = function () {
        loadingwheelmanager.end();
    };

    var reRenderTable = function (tableRowsData, onClickEventHandler, onClickDataSelector) {
        table.clear();

        for (var i = 0; i < tableRowsData.length; i++) {
            var row = table.row.add(tableRowsData[i]).node();

            if (onClickEventHandler != undefined) {
                $(row).click(getRowOnClickClosure(onClickEventHandler, onClickDataSelector(tableRowsData[i])));
            }
        }

        table.draw();
    };

    var getRowOnClickClosure = function (onClickEventHandler, data) {
        return function () {
            var alredyClicked = $(this).hasClass("highlighted-row");
            if (!alredyClicked) {
                $(this).addClass("highlighted-row");
                $(this).siblings().removeClass("highlighted-row");
                onClickEventHandler(data);
            }
        };
    };

    var addRow = function (rowData, onClickEventHandler, onClickData) {
        var row = table.row.add(rowData);
        if (onClickEventHandler != undefined) {
            row.click(function () {
                onClickEventHandler(onClickData);
            });
        }

        row.draw(false);
    };

    var removeRow = function (rowId) {
        table.row($('#' + rowId)).remove().draw(false);
    };

    var updateRow = function (rowId, newData) {
        var oldRow = table.row("#" + rowId);
        oldRow.data(newData).draw(false);
    };

    return {
        Rerender: reRenderTable,
        AddRow: addRow,
        RemoveRow: removeRow,
        UpdateRow: updateRow,
        Hide: hideTable,
        Show: showTable,
        StartLoadingWheel: startLoadingWheel,
        StopLoadingWheel: stopLoadingWheel
    };
};
