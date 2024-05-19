
$(document).ready(function () {
    var caseManagerId = getQueryParam('caseManagerId');
    loadDataTable(caseManagerId);
});

function loadDataTable(caseManagerId) {
    dataTable = $('#casesData').DataTable({
        "ajax": {
            url: '/CaseManager/ViewAllCases',
            data: { caseManagerId: caseManagerId },
            dataSrc: 'Data.$values',
        },

        "columns": [
        { data: 'RecordId'},
        { data: 'SuspectNo'},
        { data: 'OffenceCommited' },
        { data: 'Sentence' },
        { data: 'IssuedAt'},
        { data: 'IssuedBy'},
        { data: 'IssueDate'},
        { data: 'Status',
                    createdCell: function (td, cellData, rowData, row, col) {
                        $(td).css('background-color', getStatusColor(cellData));
                        
                    }},
        { 

            data: 'RecordId',
            "render": function (data) {
                return `<div class="btn-group" role="group">
                <a href="/CaseManager/Edit?RecordId=${data}" class="btn">
                <i class="bi bi-pencil-square">Edit</i></a>
                </div>`
            }
         },
        
        ]
    });
}

// Function to get query parameters from URL
function getQueryParam(name) {
    var urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}





