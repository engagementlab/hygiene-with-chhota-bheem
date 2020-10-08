import { ScullyConfig, setPluginConfig } from '@scullyio/scully';
import { baseHrefRewrite } from '@scullyio/scully-plugin-base-href-rewrite';
import './plugins/keys';

const defaultPostRenderers = ['hrefOptimise', baseHrefRewrite];
setPluginConfig(baseHrefRewrite, { href: '/hygiene/' });
export const config: ScullyConfig = {
    defaultPostRenderers,
  projectRoot: "./src",
  projectName: "client",
  outDir: './dist/static',
  routes: {
      '/stories/:key': {
          type: 'stories',
      },
      '/resources/:lang': {
          type: 'resources',
      },
  },
  inlineStateOnly: true,
};