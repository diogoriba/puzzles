fs = require('fs');


var Tree = function (parsedTree) {
	var nodeList = parsedTree;

	var getCoolness = function () {
		var coolnessList = nodeList.map(function (elem) { return elem.coolness; });
		return coolnessList.reduce(function (acc, elem) { return acc + elem; }, 0);
	};

	var getChildren = function (node) {
		return node.children.map(function (child) { return nodeList[child]; });	
	};

	return { "nodeList": nodeList, "getCoolness" : getCoolness, "getChildren": getChildren };
}

fs.readFile('tree.json', function (err, data) {
	if (err) throw err;
	var tree = new Tree(JSON.parse(data)["tree"]);
	var no = function (node) {
		var yesTree = new Tree(tree.getChildren(node).reduce(function (acc, elem) {
			return acc.concat(yes(elem));
		}, []));
		var noTree = new Tree(tree.getChildren(node).reduce(function (acc, elem) {
			return acc.concat(no(elem));
		}, [])); // need to fix this. it doesn't cover the case where one sibling is invited and the other one isn't

		/*
		  Right version is something like this:
		  ___
		  \    max(yes(child), no(child)
		  /__
		 child
		*/

		if (yesTree.getCoolness() > noTree.getCoolness()) {
			return yesTree.nodeList;
		} else {
			return noTree.nodeList;
		}
	};
	var yes = function (node) {
		var noTree = new Tree(tree.getChildren(node).reduce(function (acc, elem) {
			return acc.concat(no(elem));
		}, [node]));

		return noTree.nodeList;
	};

	var withRoot = new Tree(yes(tree.nodeList[0]));
	var withoutRoot = new Tree(no(tree.nodeList[0]));

	if (withRoot.getCoolness() > withoutRoot.getCoolness()) {
		solution = withRoot.nodeList;
	} else {
		solution = withoutRoot.nodeList;
	}
	console.log("{\"withRoot\":"+withRoot.getCoolness()+",\"withoutRoot\":"+withoutRoot.getCoolness()+",\"solution\":");
	console.log(JSON.stringify(solution));
	console.log("}");
});
