using Igor.TCP;

public class ItemMeta {
	public ItemMeta(Item item, ShopItem shopItem) {
		this.item = item;
		this.parsed = shopItem;
	}

	public Item item { get; }
	public ShopItem parsed { get; }

	public void RemoveSelf(int position) {
		ShopsScene.script.parser.RemoveItem(position);
	}
}

