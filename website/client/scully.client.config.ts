import { ScullyConfig } from '@scullyio/scully';
import './plugins/keys';

export const config: ScullyConfig = {
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