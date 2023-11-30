$(document).ready(function () {
    const host = "https://provinces.open-api.vn/api/";

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
    callAPI('https://provinces.open-api.vn/api/?depth=1');  // API các tỉnh

    var callApiDistrict = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.districts, "district1");
            });
    }

    var callApiWard = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.wards, "ward1");
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

    $("#province1").change(() => {
        console.log("Thay đổi tỉnh");
        callApiDistrict(host + "p/" + $("#province1").val() + "?depth=2");
        printResult();
    });

    $("#district1").change(() => {
        console.log("Thay đổi quận/huyện");
        callApiWard(host + "d/" + $("#district1").val() + "?depth=2");
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

});