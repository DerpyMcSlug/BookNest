var companyTable;

window.initCompanyTable = function () {

    if ($.fn.DataTable.isDataTable('#tableCompany')) return;

    companyTable = $('#tableCompany').DataTable({
        ajax: { url: '/admin/company/getall' },
        columns: [
            { data: 'name' },
            { data: 'streetAddress' },
            { data: 'city' },
            { data: 'state' },
            { data: 'postalCode' },
            { data: 'phoneNumber' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="btn-group">
                            <button class="btn btn-success btn-sm mx-1"
                                    onclick="openAdminModal('/admin/company/upsert/${data}')">
                                <i class="bi bi-pencil-square"></i>
                            </button>

                            <button class="btn btn-danger btn-sm mx-1"
                                    onclick="deleteCompany('/admin/company/delete/${data}')">
                                <i class="bi bi-trash-fill"></i>
                            </button>
                        </div>`;
                },
                width: "15%"
            }
        ],
            columnDefs: [
                { targets: -1, orderable: false, className: "text-center", width: "120px" }
            ]
    });
};

function deleteCompany(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: 'This company will be deleted!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    companyTable.ajax.reload(null, false);
                    toastr.success(data.message);
                }
            });
        }
    });
}

