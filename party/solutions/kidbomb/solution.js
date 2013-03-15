fs = require("fs");

var max = function(a, b){
	if(a > b){
		return a;
	} else {
		return b;
	}
}

var ParseNodeIncluded = function (tree, id){
	if(tree[id].children.length == 0){
		return { total: tree[id].coolness, nodes: tree[id].coolness };
	} else {
		var totalValue = tree[id].coolness;
		var nodeList = tree[id].coolness;
		for(var i = 0; i < tree[id].children.length; i++){
			excluded = ParseNodeExcluded(tree, tree[id].children[i]);
			totalValue += excluded.value;
			nodeList += excluded.nodes;

		}
		return { total: totalValue, nodes: nodeList };
	}

}


var ParseNodeExcluded = function (tree, id){
	if(tree[id].children.length == 0){
		return { total: 0, nodes: "" };
	} else {
		var totalValue = 0;
		for(var i = 0; i < tree[id].children.length; i++){
			var excluded = ParseNodeExcluded(tree, tree[id].children[i]);
			var included = ParseNodeIncluded(tree, tree[id].children[i]);
			var nodeList = "";
			if( excluded.total > included.total){
				totalValue += excluded.total;
				nodeList += ", ";
				nodeList += excluded.nodes;
			} else {
				totalValue += included.total;
				nodeList += ", "
				nodeList += included.nodes;
			}
		}
		return { total: totalValue, nodes: nodeList };
	}

}




var Parse = function(tree, id){
	excluded = ParseNodeExcluded(tree, id);
	included = ParseNodeIncluded(tree, id);
	if(excluded.total > included.total){
		return excluded.nodes;
		
	} else {
		return included.nodes;
	}
}

fs.readFile('tree.json', function (err, data) {
	var t = JSON.parse(data)["tree"];
	console.log(Parse(t, 0));
});
