function findItemInArray(array, key, valueToFind)
{
	for(var i = 0; i < array.length; i++)
	{
		if(array[i][key] === valueToFind)
		{
			return array[i];
		}
	}

	return null;
}

function showModal(id)
{
	// align modal
	var modal = $(".modal-dialog");

	// divide by 3 to make it appear further up than halfway
	modal.css("margin-top", ($(window).height() - modal.height()) / 3);

	$("#" + id).modal();
}

function getParameter(paramName)
{
	var searchString = window.location.search.substring(1),
		i, val, params = searchString.split("&");

	for (i = 0; i < params.length; i++)
	{
		val = params[i].split("=");
		if (val[0] == paramName)
		{
			return val[1];
		}
	}
	return null;
}