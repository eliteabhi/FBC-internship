export {};

declare global {
  interface Window {
    Popper: any;
    $: any;
    jQuery: any;
  }
}
