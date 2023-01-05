class autoResizeFBPost {

  constructor ($els) {
    this.$els = $els;
  }

  fixWidth ($container, $clonedContainer, doParse) {
    console.log('[FBRESIZE]: Fixing width');

    doParse = typeof doParse == 'undefined' ? true : doParse;
    const updatedWidth = $container.offsetWidth;

    $clonedContainer.querySelectorAll('.fb-post')
    .forEach((item) => {
      item.setAttribute('data-width', updatedWidth);
    });

    $container.innerHTML = $clonedContainer.innerHTML;

    if (doParse && FB && FB.XFBML && FB.XFBML.parse) {
      FB.XFBML.parse();
    }
  }

  initialize () {
    this.$els
    .forEach((item) => {
      const $container = item;
      const $clonedContainer = item.cloneNode(true);

      if (!$container.querySelectorAll('.fb-post')[0]) return false;

      this.fixWidth($container, $clonedContainer, false);

      window.addEventListener('resize', () => {
        this.fixWidth($container, $clonedContainer);
      });
    });
  }

}