$(document).ready(function () {
    const host = "https://provinces.open-api.vn/api/";

    var callAPI = (api) => {
        return axios.get(api)
            .then((response) => {
                console.log(response.data);  // In ra dữ liệu để kiểm tra
                renderData(response.data, "province");
            })
            .catch((error) => {
                console.error('Lỗi khi tải API:', error);
            });
    }

    // Gọi hàm callAPI để tải dữ liệu tỉnh khi trang web được tải
    callAPI('https://provinces.open-api.vn/api/?depth=1');  // API các tỉnh

    var callApiDistrict = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.districts, "district");
            });
    }

    var callApiWard = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.wards, "ward");
            });
    }

    var renderData = (array, select) => {
        console.log(array);  // In ra dữ liệu để kiểm tra
        let row = ' <option value="">chọn</option>';
        array.forEach(element => {
            row += `<option value="${element.code}">${element.name}</option>`;
        });
        document.querySelector("#" + select).innerHTML = row;
    }

    $("#province").change(() => {
        console.log("Thay đổi tỉnh");
        callApiDistrict(host + "p/" + $("#province").val() + "?depth=2");
        printResult();
    });

    $("#district").change(() => {
        console.log("Thay đổi quận/huyện");
        callApiWard(host + "d/" + $("#district").val() + "?depth=2");
        printResult();
    });

    $("#ward").change(() => {
        console.log("Thay đổi phường");
        printResult();
    });

    var printResult = () => {
        if ($("#district").val() != "" && $("#province").val() != "" && $("#ward").val() != "") {
            let result = $("#ward option:selected").text() + ", " + $("#district option:selected").text() + ", " + $("#province option:selected").text();
            $("#result").text(result)
        }
    }
    console.log("Chạy sau khi tải trang");

    var provinceSelect = document.getElementById("province");
    var districtSelect = document.getElementById("district");
    var wardSelect = document.getElementById("ward");

    // Kiểm tra để đảm bảo rằng các phần tử đã được tìm thấy
    console.log("provinceSelect:", provinceSelect);
    console.log("districtSelect:", districtSelect);
    console.log("wardSelect:", wardSelect);

    // Xóa thuộc tính style display
    if (provinceSelect) provinceSelect.style.display = "block";
    if (districtSelect) districtSelect.style.display = "block";
    if (wardSelect) wardSelect.style.display = "block";
});