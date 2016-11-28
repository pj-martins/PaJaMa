Rails.application.routes.draw do
scope '/api' do
    scope '/v1' do
	  get '/recipe/:id' => 'api/v1/recipe#index'
      scope '/recipes' do
        get '/' => 'api/v1/recipes#index'
		scope '/search' do
			get '/' => 'api/v1/recipes#search'
		end
=begin
		post '/' => 'api_projects#create'
        scope '/:name' do
          get '/' => 'api_projects#show'
          put '/' => 'api_projects#update'
          scope '/todos' do
            get '/' => 'api_todos#index'
            post '/' => 'api_todos#create'
            scope '/:todo_name' do
              get '/' => 'api_todos#show'
              put '/' => 'api_todos#update'
            end
          end
        end
=end
      end
	  scope '/recipeSources' do
		get '/' => 'api/v1/lookups#recipe_sources'
	  end
    end
  end
  resources :recipes, only: [:index]
  post 'recipes', action: :search, controller: 'recipes'
end 