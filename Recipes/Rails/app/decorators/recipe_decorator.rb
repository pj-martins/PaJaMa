class RecipeDecorator < Draper::Decorator
  delegate_all
  def cover_image_url
	first_image = object.RecipeImages.order(:Sequence).first;
    return first_image ? first_image.ImageURL : ''
  end
end