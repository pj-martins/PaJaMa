class RecipeDecorator < Draper::Decorator
  delegate_all
  def cover_image_url
    return object.RecipeImages.order(:Sequence).first.ImageURL
  end
end