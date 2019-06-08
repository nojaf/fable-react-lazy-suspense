// Note this only includes basic configuration for development mode.
// For a more comprehensive configuration check:
// https://github.com/fable-compiler/webpack-config-template

const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const PurgecssPlugin = require("purgecss-webpack-plugin");
const glob = require("glob");
const webpack = require("webpack");

const isProduction = !process.argv.find(
  v => v.indexOf("webpack-dev-server") !== -1
);

console.log(`Building for ${isProduction ? "production" : "development"}`);

const babel = {};

const plugins = isProduction
  ? [
      new HtmlWebpackPlugin({
        filename: "index.html",
        template: "./public/index.html"
      }),
      new MiniCssExtractPlugin({
        filename: "styles.css",
        chunkFilename: "styles.css"
      }),
      new PurgecssPlugin({
        paths: () =>
          glob.sync(`${path.join(__dirname, "src")}/**/*`, { nodir: true })
      })
    ]
  : [
      new HtmlWebpackPlugin({
        filename: "index.html",
        template: "./public/index.html"
      }),
      new webpack.HotModuleReplacementPlugin()
    ];

module.exports = {
  mode: "development",
  entry: "./src/App.fsx",
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
          isProduction ? MiniCssExtractPlugin.loader : "style-loader",
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
