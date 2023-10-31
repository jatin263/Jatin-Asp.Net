var app = angular.module("Block1", []);

app.controller("BlockController", function ($scope, $http) {
    $scope.SearchText = "";
    $scope.totalPage = 0;
    $scope.itemPerPage = 10;
    $scope.filtText = "";
    $scope.totalRows = 0;
    $scope.currentPage = 1;
    $('.loader').hide();
    $scope.getAllDivision = function () {
        $('.loader').show();
        $http.get("Master/GetMasterData?typeOfData=Division&subid=0&methodAction=o&searchText=").then(function (response) {
            //console.log(response.data.dataJson);
            $scope.divisionData = JSON.parse(response.data.dataJson);
            $('.loader').hide();
        })
    }

    $scope.setDistrict = function () {
        $('.loader').show();
        $scope.getDictrict($scope.divisionChose.DIVISION_ID)
        //console.log($scope.divisionChose);
    }

    $scope.textFilter = function () {
        $('.loader').show();
        if ($scope.filtText == "") {
            $scope.getBlock(0);
        }
        var filterData = [];
        for (var i = 0; i < $scope.blockData.length; i++) {
            if ($scope.blockData[i].district_eng.toLowerCase().includes($scope.filtText.toLowerCase()) || $scope.blockData[i].subdistrict_eng.toLowerCase().includes($scope.filtText.toLowerCase()) || $scope.blockData[i].block_eng.toLowerCase().includes($scope.filtText.toLowerCase())) {
                filterData.push($scope.blockData[i]);
                //console.log($scope.blockData[i].district_eng);
            }
        }
        $scope.blockData = filterData;
        $('.loader').hide();
    }

    $scope.getDictrict = function (id) {
        $('.loader').show();
        $http.get("Master/GetMasterData?typeOfData=District&subid=" + id +"&methodAction=o&searchText=").then(function (response) {
            //console.log(response.data.dataJson);
            $scope.districtData = JSON.parse(response.data.dataJson);
            $('.loader').hide();
        })
        $scope.getBlock(0, "");
    }

    $scope.getBlock = function (id,searchT) {
        $('.loader').show();
        var j = $scope.currentPage - 1;
        $http.get("Master/GetMasterData?typeOfData=Block&subid=" + id + "&methodAction=details&pageNum=" + j + "&itemsRow=" + $scope.itemPerPage + "&searchText=" + searchT).then(function (response) {
            //console.log(response.data.dataJson);
            $scope.blockData = JSON.parse(response.data.dataJson);
            $scope.setPagination();
            $('.loader').hide();
        })
    }
    $scope.getTotalCount = function (id, searchT) {
        //debugger;
        $('.loader').show();
        $http.get("Master/GetMasterData?typeOfData=Block&subid=" + id + "&methodAction=count&pageNum=0&itemsRow=" + $scope.itemPerPage + " & searchText=" + searchT ).then(function (response) {
            //console.log(response.data.dataJson);
            $scope.blockData = JSON.parse(response.data.dataJson);
            debugger;
            $scope.totalRows = $scope.blockData[0].count;
            $scope.totalPage = Math.ceil($scope.blockData[0].count / $scope.itemPerPage);
            console.log($scope.totalRows);
            console.log($scope.totalPage);
            $scope.setPagination();
            $('.loader').hide();
        })
    }

    setPageNumber = function (h) {
        //debugger;
        if (h == -1) {
            $scope.currentPage--;
        }
        else {
            if (h == -2) {
                $scope.currentPage++;
            }
            else {
                $scope.currentPage = h;
            }
        }
        console.log(h);
        $scope.getBlock(0, $scope.filtText);
        $scope.setPagination();
    }


    $scope.setPagination = function () {
        let f = "";
        //debugger;
        if ($scope.currentPage == 1) {
            f += `<li class="page-item disabled">
                        <a class="page-link">Previous</a>
                    </li>`;
        }
        else {
            f += `<li class="page-item">
                        <button class="page-link" onclick="setPageNumber(-1)">Previous</button>
                    </li>`;
        }
        var j = $scope.currentPage + 1;
        for (var i = $scope.currentPage - 1; i < j; i++) {
            if (i == 0) {
                i = 1;
                j = 3;
            }
            if (i == $scope.currentPage) {
                f += `<li class="page-item active"><a class="page-link" >`+i+`</a></li>`;
            }
            else {
                f += `<li class="page-item"><a class="page-link" onclick="setPageNumber(${i})" >${i}</a></li>`;
            }
        }
        f += `<li class="page-item"><a class="page-link" >....</a></li>`;
        for (var i = $scope.totalPage-1; i <= $scope.totalPage; i++) {
            if (i == $scope.currentPage) {
                f += `<li class="page-item active"><a class="page-link" >${i}</a></li>`;
            }
            else {
                f += `<li class="page-item"><a class="page-link" onclick="setPageNumber(${i})" >${i}</a></li>`;
            }
        }

        if ($scope.currentPage == $scope.totalPage) {
            f += `<li class="page-item disabled">
                        <a class="page-link">Next</a>
                    </li>`;
        }
        else {
            f += `<li class="page-item">
                        <button class="page-link" onclick="setPageNumber(-2)">Next</button>
                    </li>`;
        }
        //console.log(f);
        document.getElementById("pageNav").innerHTML = f;

    }

    //$scope.setBlock = function () {
    //    try {
    //        $scope.getBlock($scope.districtChoose.DISTRICT_ID);
    //    }
    //    catch {

    //    }
    //}

    $scope.filterData = function () {
        var searchTxt = $scope.filtText;
        let disId = 0;
        console.log($scope.filtText);
        try {
            disId=$scope.districtChoose.DISTRICT_ID;
        }
        catch {
            disId = 0;
        }
        $scope.getTotalCount(disId, searchTxt);
        $scope.getBlock(disId, searchTxt);

    }
    setItemPerPage = function(l){
        $scope.itemPerPage = parseInt(l.value);
        $scope.getBlock(0, $scope.filtText);
        $scope.getTotalCount(0, $scope.filtText);
    }

    $scope.getAllDivision();
    $scope.getDictrict(0);
    $scope.getTotalCount(0, $scope.filtText);

});