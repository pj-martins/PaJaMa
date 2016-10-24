function RecipesViewModel() {
    var self = this;
    self.recipes = ko.observableArray([]);
    $.getJSON(API_URL + '/recipeSearch/entities?random=30', function (data) {
        self.recipes(data);
    });
}

ko.applyBindings(new RecipesViewModel());