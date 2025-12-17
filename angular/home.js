var app = angular.module('AppBanHang', []);

app.controller("HomeCtrl", function ($scope, $http) {

    $scope.listloaihangMoi = [];

    // ====== LẤY SẢN PHẨM MỚI ======
    $scope.LoadloaihangMoi = function () {
        $http.get(current_url + '/api/SanPham/get-moi/20')
            .then(function (response) {
                $scope.listSanPhamMoi = response.data;
                console.log("Sản phẩm mới:", $scope.listSanPhamMoi);
                makeScript('index.js');
            })
            .catch(function (error) {
                console.error("Lỗi sản phẩm mới:", error);
            });
    };

    // ====== CHẠY KHI LOAD TRANG ======
    $scope.LoadDanhMuc();
    $scope.LoadloaihangMoi();
});
