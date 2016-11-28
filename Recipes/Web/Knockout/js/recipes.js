function RecipesViewModel() {
    var self = this;
    self.recipes = ko.observableArray([]);
    $.getJSON(API_URL + '/recipes', function (data) {
        self.recipes(data);
    });
}

ko.applyBindings(new RecipesViewModel());