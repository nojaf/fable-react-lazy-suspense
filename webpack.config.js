// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const PurgecssPlugin = require("purgecss-webpack-plugin");
const glob = require("glob");
const webpack = require("webpack");
const http = require('http');
const fs = require('fs');

const isProduction = !process.argv.find(
  v => v.indexOf("webpack-dev-server") !== -1
);

console.log(`Building for ${isProduction ? "production" : "development"}`);

const babel = {};

const defaultPlugins = [
  new MiniCssExtractPlugin({
    filename: "styles.css",
    chunkFilename: "styles.css"
  }),
  new HtmlWebpackPlugin({
    filename: "index.html",
    template: "./public/index.html"
  })
];
const plugins = isProduction
  ? [
      ...defaultPlugins,
      new PurgecssPlugin({
        paths: () =>
          glob.sync(`${path.join(__dirname, "src")}/**/*`, { nodir: true })
      })
    ]
  : [
    {
      apply: (compiler) => {
        compiler.hooks.afterCompile.tap('nojaf', (compilation) => {
          if(compilation.entrypoints.has('style')){
            console.log('DEES IS EM');
            const cssFile = fs.createWriteStream("./public/styles.css");
            http.get("http://localhost:8080/styles.css", (response, err) => {
                console.log("downloaded file");
                response.pipe(cssFile);
                console.log("written to disk")
            });
          }
        });
      }
    },
      ...defaultPlugins
  ];

module.exports = {
  mode: "development",
  entry: { 
    "app":"./src/App.fsx",
    "style": "./src/styles.pcss"
  },
  output: {
    path: path.join(__dirname, "./docs"),
    filename: "[name].bundle.js",
    chunkFilename: "[name].bundle.js",
    publicPath: "/"
  },
  devServer: {
    contentBase: "./public",
    port: 8080,
    hot: true,
    inline: true,
    historyApiFallback: true
  },
  module: {
    rules: [
      {
        test: /\.fs(x|proj)?$/,
        use: {
          loader: "fable-loader",
          options: {
            babel
          }
        }
      },
      {
        test: /\.pcss$/,
        use: [
          {
            loader: MiniCssExtractPlugin.loader,
            options: {
              hmr: !isProduction
            }
          },
          {
            loader: "css-loader",
            options: {
              importLoaders: 1
            }
          },
          {
            loader: "postcss-loader",
            options: {
              plugins: [require("tailwindcss"), require("autoprefixer")]
            }
          }
        ]
      }
    ]
  },
  optimization: {
    splitChunks: {
      chunks: "all"
    }
  },
  plugins
};
