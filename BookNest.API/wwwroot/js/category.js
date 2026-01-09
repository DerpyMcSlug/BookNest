var categoryTable;

window.loadCategoryTable = function () {

    if ($.fn.DataTable.isDataTable('#tableCategory')) {
        return;
    }

    categoryTable = $('#tableCategory').DataTable({
        ajax: {
            url: '/admin/category/getall'
        },
        columns: [
            { data: 'name' },
            { data: 'displayOrder', className: "text-center" },
            {
                data: 'id',
                render: function (data) {
                    return `
                  <div class="btn-group">
                <button onclick="openAdminModal('/admin/category/upsert/${data}')" 
                        class="btn btn-success btn-sm mx-1">
                    <i class="bi bi-pencil-square"></i>
                        </button>
                        <button onclick="deleteCategory('/admin/category/delete/${data}')" 
                            class="btn btn-danger btn-sm mx-1">
                            <i class="bi bi-trash-fill"></i>
                        </button>
                    </div>`;
                }
            }
        ],

        columnDefs: [
            {
                targets: -1,
                orderable: false,
                searchable: false,
                className: "text-center",
                width: "140px"
            }
        ]
    });
};

$(document).ready(function () {
    loadCategoryTable();
});

function deleteCategory(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This category will be deleted!",
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
                    categoryTable.ajax.reload(null, false);
                    toastr.success(data.message);
                }
            });
        }
    });
}

$(document).on("submit", "#categoryForm", function (e) {
    e.preventDefault();

    $.ajax({
        url: $(this).attr("action"),
        type: "POST",
        data: $(this).serialize(),
        success: function (res) {
            // JSON success case
            if (res && res.success) {
                bootstrap.Modal
                    .getInstance(document.getElementById("adminModal"))
                    .hide();

                categoryTable.ajax.reload(null, false);
                toastr.success("Category saved successfully");
            }
            // HTML partial returned (validation error)
            else {
                $(".modal-content").html(res);
            }
        }
    });
});