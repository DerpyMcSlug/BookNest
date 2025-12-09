var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}

function LoadDataTable() {
    dataTable = $('#tableData').DataTable({
        ajax: { url: '/admin/product/getall' },
        columns: [
            { data: 'title', width: "15%" },
            { data: 'isbn', width: "15%" },
            { data: 'listPrice', width: "10%" },
            { data: 'author', width: "15%" },
            { data: 'category.name', width: "10%" },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="w-75 btn-group">
                            <button onclick="openAdminModal('/admin/product/upsert/${data}')" class="btn btn-success mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </button>
                            <button onclick="Delete('/admin/product/delete/${data}')" class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </button>
                        </div>`;
                },
                width: "22%"
            }
        ]
    });
}

function openAdminModal(url) {
    $("#adminModal").remove(); // PREVENT DUPLICATION

    $.get(url, function (html) {
        $("#modalContainer").html(`
            <div class="modal fade" id="adminModal" tabindex="-1">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                    <div class="modal-content">${html}</div>
                </div>
            </div>
        `);

        let modal = new bootstrap.Modal(document.getElementById("adminModal"));
        modal.show();
    });
}

function submitProductForm() {
    var form = $("#productForm")[0];
    var formData = new FormData(form);

    $.ajax({
        url: "/admin/product/upsert",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            if (res.success) {
                $("#adminModal").modal("hide");
                dataTable.ajax.reload();
                toastr.success("Product saved successfully");
            } else {
                $("#adminModal .modal-content").html(res.html);
            }
        }
    });
}