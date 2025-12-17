var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/Admin/Product/GetAll' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'price', "width": "15%" },
            {
                data: 'stock', "width": "10%",
                "render": function (data) {
                    if (data < 10) {
                        return `<span class="text-danger fw-bold">${data}</span>`;
                    }
                    return data;
                }
            },
            { data: 'category.name', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Düzenle</a>               
                     <a onClick=Delete('/Admin/Product/Delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Sil</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bunu geri alamazsınız!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, sil!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
