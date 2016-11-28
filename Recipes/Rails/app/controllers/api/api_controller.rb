﻿class Api::ApiController < ApplicationController
	respond_to :json
	after_filter :cors_set_access_control_headers

	def cors_set_access_control_headers
	  headers['Access-Control-Allow-Origin'] = '*'
	  headers['Access-Control-Allow-Methods'] = 'POST, PUT, DELETE, GET, OPTIONS'
	  headers['Access-Control-Request-Method'] = '*'
	  headers['Access-Control-Allow-Headers'] = 'Origin, X-Requested-With, Content-Type, Accept, Authorization'
	end

end
