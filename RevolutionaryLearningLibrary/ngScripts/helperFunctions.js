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
	modal.css("margin-top", Math.max(0, ($(window).height() - modal.height()) / 2));

	$("#" + id).modal();
}