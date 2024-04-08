$(document).ready(function () {
    const host = "https://localhost:7064/api/";

    var callAPI = (api) => {
        return axios.get(api)
            .then((response) => {
                console.log(response.data);  // In ra dữ liệu để kiểm tra
                renderData(response.data, "province1");
            })
            .catch((error) => {
                console.error('Lỗi khi tải API:', error);
            });
    }

    // Gọi hàm callAPI để tải dữ liệu tỉnh khi trang web được tải
    callAPI('https://localhost:7064/api/Province');  // API các tỉnh

    var callApiDistrict = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data, "district1");
            });
    }

    var callApiWard = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data, "ward1");
            });
    }

    var renderData = (array, select) => {
        console.log(array);  // In ra dữ liệu để kiểm tra
        let row = ' <option value="">chọn</option>';
        array.forEach(element => {
            row += `<option value="${element.code}">${element.fullName}</option>`;
        });
        document.querySelector("#" + select).innerHTML = row;
    }

    $("#province1").change(() => {
        console.log("Thay đổi tỉnh");
        $("#district1").html("<option value=''>Chọn</option>");
        $("#ward1").html("<option value=''>Chọn</option>");
        callApiDistrict(host + "District/" + $("#province1").val() + "");
        printResult();
    });

    $("#district1").change(() => {
        console.log("Thay đổi quận/huyện");
        $("#ward1").html("<option value=''>Chọn</option>");
        callApiWard(host + "Ward/" + $("#district1").val() + "");
        printResult();
    });

    $("#ward1").change(() => {
        console.log("Thay đổi phường");
        printResult();
    });

    var printResult = () => {
        if ($("#district1").val() != "" && $("#province1").val() != "" && $("#ward1").val() != "") {
            let result = $("#ward1 option:selected").text() + ", " + $("#district1 option:selected").text() + ", " + $("#province1 option:selected").text();
            $("#result").text(result)
        }
    }
    console.log("Chạy sau khi tải trang");

    var provinceSelect = document.getElementById("province1");
    var districtSelect = document.getElementById("district1");
    var wardSelect = document.getElementById("ward1");

    // Kiểm tra để đảm bảo rằng các phần tử đã được tìm thấy
    console.log("provinceSelect:", provinceSelect);
    console.log("districtSelect:", districtSelect);
    console.log("wardSelect:", wardSelect);
    // Xóa thuộc tính style display
    if (provinceSelect) provinceSelect.style.display = "block";
    if (districtSelect) districtSelect.style.display = "block";
    if (wardSelect) wardSelect.style.display = "block";
    // Lấy phần tử select có id là "ward1"
    var selectElement = document.getElementById('ward1');

    // Kiểm tra xem phần tử có tồn tại và có phần tử em (sibling) tiếp theo không
    if (selectElement && selectElement.nextElementSibling) {
        // Lấy phần tử em (sibling) tiếp theo và xóa nó khỏi DOM
        var nextElement = selectElement.nextElementSibling;
        nextElement.parentNode.removeChild(nextElement);
    }

});