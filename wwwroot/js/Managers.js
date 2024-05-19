$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#managerData').DataTable({
        "ajax": {
            url: '/CaseManager/GetAllManagers'
        },

        "columns": [
        { data: 'firstName' },
        { data: 'lastName' },
        { 

            data: 'id',
            "render": function (data) {
                return `<div class="btn-group" role="group">
                <a href="/CaseManager/ViewCase?caseManagerId=${data}" class="btn">View</a>
                </div>`
            }
         },
        //{ data: 'Cases', "width": "15%" },
        //{ data: 'IssuedAt', "width": "15%" },
        //{ data: 'IssuedBy', "width": "15%"  },
        //{ data: 'DateIssued', "width": "15%"  },
        //{ data: 'Edit', "width": "15%"  },
        ]
    });
}



